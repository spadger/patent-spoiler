///<reference path="../module.js" />
///<reference path="~/scripts/custom/_common/factories/recursionHelper.js" />
'use strict';
angular.module('utils').directive('categoryHierrachyView', ['recursionHelper', function (recursionHelper) {
    return {
        restrict: 'AE',
        scope: {
            tree: '=',
            mode: '@',
            selectedItems: '=',
            getIdGenerator: '&idGenerator'
        },
        controller: function($scope) {

            $scope.entityId = ($scope.getIdGenerator() || function (x) { return x.Id; })($scope.tree);
            debugger
            $scope.selected = false;
            $scope.show = true;
            $scope.symbol = '-';
            $scope.toggleExpansion = function () {
                $scope.show = !$scope.show;
                $scope.symbol = $scope.show ? '-' : '+';
            };

            $scope.onItemToggled = function (selected) {
                if (selected) {
                    $scope.selectedItems[$scope.entityId] = $scope.tree;
                } else {
                    delete $scope.selectedItems[$scope.entityId];
                }
            }
        },
        compile: function(element) {
            // Use the compile function from the RecursionHelper,
            // And return the linking function(s) which it returns
            return recursionHelper.compile(element);
        },
        templateUrl: function (element, attrs) {
            return attrs.templateUrl;
        }
    }
}]);