using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ActivityStreams.Helpers;

namespace ActivityStreams
{
    [DebuggerDisplay("Id = {System.Text.Encoding.UTF8.GetString(StreamId)} ExpiresAt = {System.DateTime.FromFileTimeUtc(ExpiresAt)}")]
    public class ActivityStream : IEqualityComparer<ActivityStream>, IEquatable<ActivityStream>
    {
        public const long DefaultExpirationTimestamp = 2650467743999999999; // DateTime.MaxValue.ToFileTimeUtc();

        public ActivityStream(byte[] streamId, long expiresAt = DefaultExpirationTimestamp)
        {
            StreamId = streamId;
            AttachedStreams = new HashSet<ActivityStream>();
            ExpiresAt = expiresAt;
        }

        [DebuggerDisplay("{System.Text.Encoding.UTF8.GetString(StreamId)}")]
        public byte[] StreamId { get; set; }

        [DebuggerDisplay("{System.DateTime.FromFileTimeUtc(ExpiresAt)}")]
        public long ExpiresAt { get; set; }

        public bool HasExpiration { get { return ExpiresAt != DefaultExpirationTimestamp; } }

        public HashSet<ActivityStream> AttachedStreams { get; set; }

        public IEnumerable<ActivityStream> Streams { get { return AttachedStreams.ToList().AsReadOnly(); } }

        public static ActivityStream Empty = new ActivityStream(Guid.Empty.ToByteArray());

        public static bool IsEmpty(ActivityStream stream)
        {
            return stream.Equals(Empty);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as StreamService);
        }

        public bool Equals(ActivityStream other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var t = GetType();
            if (t != other.GetType())
                return false;

            return ByteArrayHelper.Compare(StreamId, other.StreamId);
        }

        public bool Equals(ActivityStream left, ActivityStream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public int GetHashCode(ActivityStream obj)
        {
            return obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 463;
                int multiplier = 11;

                var streamIdHash = ByteArrayHelper.ComputeHash(StreamId);
                hashCode = hashCode * multiplier ^ streamIdHash;

                return hashCode;
            }
        }

        public Result<bool> Attach(byte[] streamId, long expiresAt = DefaultExpirationTimestamp)
        {
            var streamToAttach = new ActivityStream(streamId, expiresAt);
            if (AttachedStreams.Add(streamToAttach))
                return Result.Success;
            return Result.Error("Stream is already attached");
        }

        public Result<bool> Detach(byte[] streamId, DateTime detachedSince)
        {
            var streamToDetach = new ActivityStream(streamId);
            var detachedStream = AttachedStreams.Where(x => x.StreamId.Equals(streamId)).SingleOrDefault() ?? ActivityStream.Empty;

            if (IsEmpty(detachedStream))
                return Result.Error("No stream found to detach.");

            if (detachedStream.HasExpiration)
                return Result.Error($"Stream is already detached since {DateTime.FromFileTimeUtc(ExpiresAt).ToShortDateString()}.");

            detachedStream.ExpiresAt = detachedSince.ToFileTimeUtc();
            return Result.Success;
        }

        public static bool operator ==(ActivityStream left, ActivityStream right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left))
                return false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(ActivityStream left, ActivityStream right)
        {
            return !(left == right);
        }
    }

    public struct Result<T>
    {
        List<string> errors;

        public Result(T value)
        {
            Value = value;
            errors = null;
        }

        public T Value { get; }

        public bool IsSuccessful { get { return ReferenceEquals(null, errors) || errors.Count == 0; } }

        public IEnumerable<string> Errors
        {
            get
            {
                return ReferenceEquals(null, errors) ? Enumerable.Empty<string>() : errors.AsReadOnly();
            }
        }

        public Result<T> WithError(string errorMessage)
        {
            var newErrors = ReferenceEquals(null, errors) ? new List<string>() : new List<string>(errors);
            newErrors.Add(errorMessage);
            var newResult = new Result<T>();
            newResult.errors = newErrors;
            return newResult;
        }

        public Result<T> WithError(IEnumerable<string> errorMessages)
        {
            var newErrors = ReferenceEquals(null, errors) ? new List<string>() : new List<string>(errors);
            newErrors.AddRange(errorMessages);
            var newResult = new Result<T>();
            newResult.errors = newErrors;
            return newResult;
        }
    }

    public static class Result
    {
        public static Result<bool> Success = new Result<bool>(true);

        public static Result<bool> Error(string errorMessage)
        {
            return new Result<bool>().WithError(errorMessage);
        }

        public static Result<bool> Error(IEnumerable<string> errorMessages)
        {
            return new Result<bool>().WithError(errorMessages);
        }
    }
}