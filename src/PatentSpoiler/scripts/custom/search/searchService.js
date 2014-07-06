///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />
///<reference path="searchController.js" />
'use strict';
angular.module('search').factory('searchService', ['$http', '$q', function($http, $q) {

    return {
        performSearch: function (term) {

            var deferred = $q.defer();
            $http({ method: 'GET', url: '/search/for', params: { term: term } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
}]);