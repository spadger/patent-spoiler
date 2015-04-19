///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
'use strict';
angular.module('account').factory('accountService', ['$http', '$q', function($http, $q) {

    return {
        resendEmailVerificationCode: function () {

            var deferred = $q.defer();

            $http({ method: 'POST', url: '/account/resendEmailVerificationCode' })
                .success(function(result) {
 
                    if (result.success) {
                        deferred.resolve();
                    } else {
                        deferred.reject(result.errors);
                    }
                })
                .error(function() {
                    deferred.reject({ general: ['Sorry, something went wrong whilst sending your email.'] });
                }
            );

            return deferred.promise;
        }
    }
}]);