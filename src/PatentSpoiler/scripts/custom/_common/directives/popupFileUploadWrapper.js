///<reference path="../module.js" />

'use strict';
angular.module('utils').directive('popupFileUploadWrapper', ['$modal', function ($modal) {
    return {
        restrict: 'AE',
        scope: {
            existingFiles: '&',
            successCallback: '&'
        },
        template: '<button class="btn" type="button" ng-click="open()">Choose Attachments</button>',
        controller: function($scope) {
            
            $scope.open = function () {

                var extraArgs = {
                    existingFiles: $scope.existingFiles().slice(0),
                    successCallback: $scope.successCallback()
                }

                var modalInstance = $modal.open({
                    size:'lg',
                    templateUrl: '/scripts/custom/_common/directives/popupFileUploadWrapperView.html',
                    resolve: {
                        extraArgs: function() {
                            return extraArgs;
                        }
                    },
                    controller: popupFileUploaderInstanceController
                });
            }
        }
    }
}]);

var popupFileUploaderInstanceController = function ($scope, $modalInstance, extraArgs) {
    
    $scope.args = extraArgs;
    $scope.modalInstance = $modalInstance; //getting NRE

    $scope.ok = function() {
        $scope.$broadcast('ok');
    }

    $scope.$on('filesAmended', function (event) {
        event.stopPropagation();
        $scope.modalInstance.close();
    });

    $scope.cancel = function () {
        $scope.modalInstance.dismiss();
    };
};