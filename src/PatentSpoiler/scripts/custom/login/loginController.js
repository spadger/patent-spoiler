///<reference path="module.js" />
/// <reference path="loginService.js" />
'use strict';
angular.module('login').controller('loginController', ['$scope', '$window', 'queryString', 'loginService', function ($scope, $window, queryString, loginService) {

    $scope.submitted = false;
    $scope.working = false;
    $scope.mode = 'login';
    $scope.serverErrors = [];
    
    $scope.switchToRegister = function () {

        $scope.loginForm.$setPristine();
        $scope.submitted = false;
        $scope.mode = 'register';
    }

    $scope.switchToLogin = function () {
        $scope.loginForm.$setPristine();
        $scope.submitted = false;
        $scope.mode = 'login';
    }

    var redirectToUrlOfDefault = function () {
        var returnUrl = queryString.get().ReturnUrl || '/me';
        $window.location.href = $window.decodeURIComponent(returnUrl);
    }

    var login = function () {
        loginService.login($scope.username, $scope.password, $scope.rememberMe)
                .then(redirectToUrlOfDefault,
                function (errors) {
                    $scope.serverErrors = errors;
                });
    }

    var register = function () {
        loginService.register($scope.username, $scope.email, $scope.password, $scope.passwordConfirmation, $scope.rememberMe)
               .then(redirectToUrlOfDefault,
               function (errors) {
                   $scope.serverErrors = errors;
               });
    }

    $scope.submit = function () {
        
        if (!$scope.loginForm.$valid) {
            $scope.submitted = true;
            $scope.loginForm.$setPristine();
            return;
        }
        
        if ($scope.mode === 'login') {
            login();
        } else {
            register();
        }
    }
    
}]);