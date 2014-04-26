///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />

window.homeApp.controller('HomeController', ['$scope', 'searchService', function($scope, searchService) {

    var searchSuccess = function (results) {
        $scope.searchResults = results;
    }

    var searchFailed = function () {

    }
    $scope.term = 'garden';
    $scope.performSearch = function() {
        searchService.performSearch($scope.term, searchSuccess, searchFailed);
    }

}]);