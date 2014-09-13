///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />
'use strict';
angular.module('user').factory('userPatentService', ['$http', '$q', function ($http, $q) {

    var self = this;

   

    return {
        morePatents: function (skip, user) {

            var deferred = $q.defer();
            $http({ method: 'GET', url: '/user/' + user + '/patents/', params: { skip: skip } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    };
}]);