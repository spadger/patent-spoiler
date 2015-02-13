///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
'use strict';
angular.module('login').factory('loginService', ['$http', '$q', function($http, $q) {

    return {
        login: function (username, password, rememberMe) {

            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/login', data: { username: username, password: password, rememberMe: rememberMe } })
                .success(function(result) {
 
                    if (result.success) {
                        deferred.resolve();
                    } else {
                        deferred.reject(result.errors);
                    }
                })
                .error(function() {
                     deferred.reject({ general: ['Sorry, something went wrong whilst logging you in.'] });
            });

            return deferred.promise;
        },
        register: function (username, email, password, passwordConfirmation, rememberMe) {

            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/register', data: { username: username, email: email, password: password, passwordConfirmation: passwordConfirmation, rememberMe: rememberMe } })
                 .success(function (result) {

                     if (result.success) {
                         deferred.resolve();
                     } else {
                         deferred.reject(result.errors);
                     }
                 })
                .error(function () {
                    deferred.reject({ general: ['Sorry, something went wrong whilst registering your account.'] });
                });

            return deferred.promise;
        },
        beginResetPassword: function(account) {
            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/BeginForgottenPassword', data: { account: account} })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        },
        confirmForgottenPassword : function(password, passwordConfirmation, token) {
            debugger
            var deferred = $q.defer();

            $http({ method: 'PUT', url: '/account/confirmForgottenPassword', data: { password: password, passwordConfirmation: passwordConfirmation, token: token } })
                .success(function (result) {

                    if (result.success) {
                        deferred.resolve();
                    } else {
                        deferred.reject(result.errors);
                    }
                })
                .error(function () {
                    deferred.reject({ general: ['Sorry, something went wrong whilst logging you in.'] });
                });

            return deferred.promise;
        }
    }
}]);