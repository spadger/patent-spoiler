///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
'use strict';
angular.module('item').factory('itemService', ['$http', '$q', function($http, $q) {

    return {
        addItem: function (item) {


            var payload = angular.copy(item);
            var categoryIds = _.map(payload.categories, function (x) { return x.Id; });
            payload.categories = categoryIds;

            var deferred = $q.defer();
            $http({ method: 'POST', url: '/item/add', data: { item: payload } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        },
        updateItem: function (item) {


        var payload = angular.copy(item);
        var categoryIds = _.map(payload.categories, function (x) { return x.Id; });
        payload.categories = categoryIds;

        var deferred = $q.defer();
        $http({ method: 'PUT', url: '/item/' + item.id, data: { item: payload } })
            .success(deferred.resolve)
            .error(deferred.reject);

        return deferred.promise;
    }
    }
}]);