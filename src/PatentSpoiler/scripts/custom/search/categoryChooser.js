﻿/// <reference path="~/scripts/custom/search/app.js" />
/// <reference path="~/scripts/custom/search/searchService.js" />

'use strict';
angular.module('search').directive('categoryChooser', ['searchService', function (searchService) {
    return {
        restrict: 'AE',
        scope: {
            selectedItems: '='            
        },
        templateUrl: '/scripts/custom/search/categoryChooserView.html',
        controller: function($scope) {

            $scope.working = false;

            var searchSuccess = function (results) {
                $scope.searchResults = results;
            }

            var searchFailed = function (error) {
                alert('ruh-ro');
            }

            $scope.performSearch = function () {
                $scope.working = true;
                searchService.performSearch($scope.term).then(searchSuccess, searchFailed).finally(function () { $scope.working = false; });
            }
        }
    }
}]);