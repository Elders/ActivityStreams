#### 0.5.0 - 14.10.2016
* Added ability to remove activity from stream
* Fix attach a stream with an expireation date
* Allow to attach a stream with an expireation date
* Add asc load of Activities
* Now streams are detached with (expiration/detach) timestamp. Activities after that date are not loaded.

#### 0.4.0 - 17.10.2015
* Added SortOrder for activities.

#### 0.3.0 - 12.10.2015
* Changed the accessibility level of Activity constructor
* Added  the Load of Activity to take Paging type instead of DateTime

#### 0.2.0 - 05.10.2015
* Ability to attach/detach feed to feed or stream to feed
* Rollback the C* driver because of a bug
* Fixed streamId persistence in the feeds
* Add Cassandra persistence

#### 0.1.0 - 20.07.2015
* Initial release
