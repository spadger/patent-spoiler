///<reference path="app.js" />
'use strict';
angular.module('search').controller('SearchController', ['$scope', '$window', 'searchService', function($scope, $window, searchService) {

    var searchSuccess = function (results) {
        $scope.searchResults = results;
    }

    var searchFailed = function (error) {
        alert('ruh-ro');
    }

    $scope.term = $window.term || 'garden';
    $scope.performSearch = function() {
        searchService.performSearch($scope.term).then(searchSuccess, searchFailed);
    }

    if (!!window.term) {
        $scope.performSearch();
    }
}]);