///<reference path="app.js" />
'use strict';
angular.module('search').controller('SearchController', ['$scope', '$window', 'searchService', function($scope, $window, searchService) {

    $scope.working = false;
    $scope.selectedItems = {};
    var searchSuccess = function (results) {
        $scope.searchResults = results;
    }

    var searchFailed = function (error) {
        alert('ruh-ro');
    }

    $scope.term = $window.term || 'garden';
    $scope.performSearch = function () {
        $scope.working = true;
        searchService.performSearch($scope.term).then(searchSuccess, searchFailed).finally(function () { $scope.working = false; });
    }

    if (!!window.term) {
        $scope.performSearch();
    }
}]);