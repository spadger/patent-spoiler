///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
///<reference path="addItemController.js" />
'use strict';
angular.module('item').factory('addItemService', ['$http', '$q', function($http, $q) {

    return {
        AddItem: function (item) {

            var deferred = $q.defer();
            $http({ method: 'POST', url: '/item/add', params: { item: item } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
}]);