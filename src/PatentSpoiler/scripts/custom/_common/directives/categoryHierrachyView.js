///<reference path="../module.js" />
///<reference path="~/scripts/custom/_common/factories/recursionHelper.js" />
'use strict';
angular.module('utils').directive('categoryHierrachyView', ['recursionHelper', function (recursionHelper) {
    return {
        restrict: 'AE',
        scope: {
            tree: '=',
            mode: '@',
            topic: '@'
        },
        controller: function($scope) {

            $scope.selected = false;
            $scope.show = true;
            $scope.symbol = '-';
            $scope.toggleExpansion = function () {
                $scope.show = !$scope.show;
                $scope.symbol = $scope.show ? '-' : '+';
            };

            $scope.itemChecked = function (selected) {
                $scope.$emit($scope.topic, {selected:selected, item: $scope.tree});
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