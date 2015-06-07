///<reference path="app.js" />
'use strict';
angular.module('search').controller('SearchController', ['$scope', '$window', 'searchService', function($scope, $window, searchService) {

    $scope.working = false;
    $scope.searchResults = [];
    $scope.searchResultCount = 0;
    $scope.prevSearch = '';
    
    $scope.term = $window.term || 'garden';
    $scope.performSearch = function () {
        $scope.working = true;
        $scope.searchResults = [];
        $scope.moreItems();
    }

    $scope.moreItems = function() {

        if ($scope.prevSearch !== $scope.term) {
            $scope.searchResults = [];
        }
        searchService.searchByClassification($scope.term, $scope.searchResults.length)
                     .then(searchSuccess, searchFailed)
                     .finally(function () { $scope.working = false; });
    }

    var searchSuccess = function (results) {

        $scope.searchResults = $scope.searchResults.concat(results.items);
        $scope.searchResultCount = results.count;
        $scope.prevSearch = $scope.term;
    }

    var searchFailed = function (error) {
        alert('ruh-ro');
    }

    $scope.moreResultsAvailable = function() {
        return $scope.searchResultCount > 0 && $scope.searchResultCount > $scope.searchResults.length;
    }
    
    if (!!window.term) {
        $scope.performSearch();
    }
}]);