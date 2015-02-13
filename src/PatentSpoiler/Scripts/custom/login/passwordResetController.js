///<reference path="module.js" />
/// <reference path="loginService.js" />
'use strict';
angular.module('login').controller('passwordResetController', ['$scope', '$window', 'loginService', function ($scope, $window, loginService) {

    $scope.submitted = false;
    $scope.working = false;
    $scope.serverErrors = {};
    $scope.token = $window.token;

    $scope.resetPassword = function () {

        $scope.submitted = true;
        $scope.serverErrors = {};
        $scope.working = true;
        
        if (!$scope.passwordResetForm.$valid) {
            $scope.passwordResetForm.$setPristine();
            $scope.working = false;
            return;
        }
        
        loginService.confirmForgottenPassword($scope.password, $scope.passwordConfirmation, $scope.token)
                    .then(function() {
                        $window.location.href = '/signin';
            },
                    function (errors) {
                        $scope.serverErrors = errors;
                    }).finally(function () {
                        $scope.working = false;
                    });
    }
}]);