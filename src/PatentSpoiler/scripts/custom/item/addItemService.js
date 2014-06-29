///<reference path="~/scripts/angular.js" />
///<reference path="module.js" />
///<reference path="addItemController.js" />
'use strict';
angular.module('item').factory('addItemService', ['$http', '$q', function($http, $q) {

    return {
        addItem: function (item, category) {

            var deferred = $q.defer();
            $http({ method: 'POST', url: '/item/add/' + category, data: { item: item } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        }
    }
}]);