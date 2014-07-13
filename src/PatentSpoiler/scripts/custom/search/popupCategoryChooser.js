/// <reference path="~/scripts/custom/search/app.js" />

'use strict';
angular.module('search').directive('popupCategoryChooser', [, function (searchService) {
    return {
        restrict: 'AE',
        scope: {
            selectedItems: '='            
        },
        templateUrl: '/scripts/custom/search/popupCategoryChooserView.html',
        controller: function($scope) {

            $scope.workingItems = $scope.selectedItems.slice(0);
            $scope.active = false;

            $scope.open = function () {
                alert('open');
            }

            $scope.close = function () {
                alert('close');
            }

            $scope.ok = function () {
                alert('ok');
                $scope.selectedItems = $scope.workingItems;
                $scope.close();
            }

            
        }
    }
}]);