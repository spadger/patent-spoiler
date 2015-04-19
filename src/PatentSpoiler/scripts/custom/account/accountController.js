///<reference path="module.js" />
/// <reference path="accountService.js" />
'use strict';
angular.module('account').controller('accountController', ['$scope', 'accountService', function ($scope, accountService) {

    $scope.working = false;
    $scope.errorMessage = '';
    $scope.status = null;

    $scope.resendEmailVerificationCode = function () {
        $scope.working = true;
        $scope.status = null;
        accountService.resendEmailVerificationCode()
                .then(function() {
                    $scope.status = true;
                },
                function (errorMessage) {
                    $scope.status = false;
                    $scope.errorMessage = errorMessage;
                }).finally(function () {
                    $scope.working = false;
                });
    }
}]);