///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />
///<reference path="homeController.js" />
'use strict';
window.homeApp.factory('searchService', ['$http', '$q', function($http, $q) {

    return {
        performSearch: function (term) {

            var deferred = $q.defer();
            $http({ method: 'GET', url: '/search', params: { term: term } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
}]);