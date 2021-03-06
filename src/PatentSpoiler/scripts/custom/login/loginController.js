﻿///<reference path="module.js" />
/// <reference path="loginService.js" />
'use strict';
angular.module('login').controller('loginController', ['$scope', '$window', 'queryString', 'loginService', '$modal', function ($scope, $window, queryString, loginService, $modal) {

    $scope.submitted = false;
    $scope.working = false;
    $scope.mode = 'login';
    $scope.serverErrors = {};
    
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
                }).finally(function() {
                    $scope.working = false;
                });
    }

    var register = function () {
        loginService.register($scope.username, $scope.email, $scope.password, $scope.passwordConfirmation, $scope.rememberMe)
               .then(redirectToUrlOfDefault,
               function (errors) {
                   $scope.serverErrors = errors;
               }).finally(function() {  
                   $scope.working = false;
               });
    }

    $scope.submit = function () {

        $scope.submitted = true;
        $scope.serverErrors = {};
        $scope.working = true;
        $scope.loginForm.$setPristine();
        if (!$scope.loginForm.$valid) {
            $scope.loginForm.$setPristine();
            $scope.working = false;
            return;
        }
        
        if ($scope.mode === 'login') {
            login();
        } else {
            register();
        }
    }

    $scope.forgotPassword = function() {
        $modal.open({
            templateUrl: 'forgotPassword.html',
            resolve: {
                account: function () {
                    return $scope.username;
                }
            },
            controller: function ($scope, $modalInstance, account) {
                
                $scope.account = account;
                $scope.ok = $modalInstance.close;
                $scope.sent = false;
                $scope.working = false;

                $scope.tryReset = function () {
                    $scope.sent = false;
                    $scope.working = true;

                    loginService.beginResetPassword($scope.account)
                                .then(function () { $scope.sent = true; },
                                function (){ alert('Sorry, something went wrong')})
                                .finally(function() { $scope.working = false; });
                }
            }
        });
    }
}]);