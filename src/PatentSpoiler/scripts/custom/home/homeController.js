///<reference path="app.js" />
'use strict';
window.homeApp.controller('HomeController', ['$scope', 'searchService', function($scope, searchService) {

    var searchSuccess = function (results) {
        $scope.searchResults = results;
    }

    var searchFailed = function (error) {
        alert('ruh-ro');
    }

    $scope.term = 'garden';
    $scope.performSearch = function() {
        searchService.performSearch($scope.term).then(searchSuccess, searchFailed);
    }

}]);