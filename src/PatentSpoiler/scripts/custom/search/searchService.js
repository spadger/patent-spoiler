﻿///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />
///<reference path="searchController.js" />
'use strict';
angular.module('search').factory('searchService', ['$http', '$q', function($http, $q) {

    var self = this;

    self.transformTreeIntoSelectableCategories = function(tree) {

        var results = [];

        for (var i = 0; i < tree.length; i++) {
            var childNode = self.getChildNode(tree[i]);
            results.push({ id: childNode.Id, hierrachy:tree[i]});
        }

        return results;
    }

    self.getChildNode = function(node) {

        while (node.Child != null) {
            node = node.Child;
        }

        return node;
    }

    var service = {
        performSearch: function(term) {

            var deferred = $q.defer();
            $http({ method: 'GET', url: '/search/for', params: { term: term } })
                .success(deferred.resolve)
                .error(deferred.reject);

            return deferred.promise;
        },
        performSearchForCategorySelection: function(category, page) {
            var deferred = $q.defer();

            var searchPromise = service.performSearch(category, page);

            searchPromise.then(function(results) {
                
                results = self.transformTreeIntoSelectableCategories(results);
                deferred.resolve(results);
            }, deferred.reject);


            return deferred.promise;
        }
    };

    return service;
}]);