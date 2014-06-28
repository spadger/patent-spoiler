///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
'use strict';
angular.module('category').factory('categoryListingService', ['$http', '$q', function($http, $q) {

    return {
        performSearch: function (category, page) {

            var deferred = $q.defer();
            $http({ method: 'GET', url: '/category/list/' + category, params: { page: page } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
}]);