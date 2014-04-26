﻿///<reference path="~/scripts/custom/_common/factories/recursionHelper.js"/>

function registerHierachyViewDirective(module) {
    module.directive('hierrachyView', ['recursionHelper', function (recursionHelper) {
        return {
            restrict: 'AE',
            scope: {
                tree: '='
            },
            controller: function($scope) {

                $scope.show = true;
                $scope.symbol = '-';
                $scope.za = function () {
                    $scope.show = !$scope.show;
                    $scope.symbol = $scope.show ? '-' : '+';
                };
            },
            compile: function(element) {
                // Use the compile function from the RecursionHelper,
                // And return the linking function(s) which it returns
                return recursionHelper.compile(element);
            },
            templateUrl: '/scripts/custom/_common/directives/hierrachyView.html'
        }
    }]);
}