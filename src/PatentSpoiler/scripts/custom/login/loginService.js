///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
'use strict';
angular.module('login').factory('loginService', ['$http', '$q', function($http, $q) {

    return {
        login: function (username, password, rememberMe) {

            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/login', data: { username: username, password: password, rememberMe: rememberMe } })
                .success(function(result) {
 
                    if (result.ok) {
                        deferred.resolve();
                    } else {
                        deferred.reject(result.errors);
                    }
                })
                .error(function() {
                     deferred.reject(['Sorry, something went wrong whilst logging you in.']);
            });

            return deferred.promise;
        },
        register: function (username, email, password, passwordConfirmation, rememberMe) {

            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/register', data: { username: username, email: email, password: password, passwordConfirmation: passwordConfirmation, rememberMe: rememberMe } })
                .success(deferred.resolve)
                .error(function () {

                    deferred.reject(['Sorry, something went wrong whilst registering your account.']);
                });

            return deferred.promise;
        }
    }
}]);