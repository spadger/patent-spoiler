/// <reference path="~/scripts/custom/search/app.js" />

'use strict';
angular.module('search').directive('popupCategoryChooser', ['$modal', function ($modal) {
    return {
        restrict: 'AE',
        scope: {
            selectedItems: '='            
        },
        templateUrl: '/scripts/custom/search/popupCategoryChooserView.html',
        controller: function($scope) {
            
            $scope.open = function () {

                $scope.workingItems = {};

                var modalInstance = $modal.open({
                    templateUrl: '/scripts/custom/search/popupCategoryChooserPopupView.html',
                    resolve: {
                        workingItems: function () {
                            return $scope.workingItems;
                        }
                    },
                    controller: popupInstanceController
                });

                modalInstance.result.then(function () {
                    for (var prop in $scope.workingItems) {
                        $scope.selectedItems[prop] = $scope.workingItems[prop];
                    }
                });
            }
        }
    }
}]);

var popupInstanceController = function($scope, $modalInstance, workingItems) {

    $scope.selectedItems = workingItems;
    $scope.$modalInstance = $modalInstance; //getting NRE
    $scope.ok = function(modal) {
        modal.close();
    };

    $scope.cancel = function (modal) {
        modal.dismiss();
    };
};