'use strict';

angular.module('tillDempApp.version', [
  'tillDempApp.version.interpolate-filter',
  'tillDempApp.version.version-directive'
])

.value('version', '0.1');
