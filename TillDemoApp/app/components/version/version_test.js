'use strict';

describe('tillDempApp.version module', function() {
  beforeEach(module('tillDempApp.version'));

  describe('version service', function() {
    it('should return current version', inject(function(version) {
      expect(version).toEqual('0.1');
    }));
  });
});
