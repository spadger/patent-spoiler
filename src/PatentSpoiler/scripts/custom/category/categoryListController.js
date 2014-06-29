///<reference path="module.js" />
///<reference path="categoryListingService.js" />
'use strict';
angular.module('category').controller('CategoryListController', ['$scope', '$window', 'categoryListingService', function ($scope, $window, categoryListingService) {


    $scope.page = 1;
    $scope.items = [];
    $scope.itemCount = null;
    $scope.working = false;
    $scope.category = $window.category;

    //$scope.moreItemsAllowed = false;
    
    $scope.moreItems = function () {
        $scope.working = true;
        categoryListingService.performSearch($scope.category, $scope.page)
                                .then(gotMoreItems, error)
                                .finally(function() {
                                        $scope.working = false;
                                    });
    }

    var gotMoreItems = function (results) {
        $scope.page++;
        if (!$scope.itemCount) {
            $scope.itemCount = results.count;
        }
        $scope.items.push.apply($scope.items, results.items);
    }

    $scope.noItems = function () {
        return $scope.itemCount === 0;
    }

    $scope.moreItemsAllowed = function () {
        return $scope.itemCount > 0 && $scope.items.length < $scope.itemCount;
    }

    var error = function() {
        alert('rur-roh');
    }

    $scope.moreItems();

}]);