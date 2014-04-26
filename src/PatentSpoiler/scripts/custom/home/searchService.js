///<reference path="~/scripts/angular.js" />
///<reference path="app.js" />
///<reference path="homeController.js" />

window.homeApp.factory('searchService', ['$http', function($http) {

    return {
        performSearch: function (term, onSuccess, onError) {
            $http({ method: 'GET', url: '/search', params:{term:term} })
            .success(onSuccess)
            .error(onError);
        }
    }
    
}]);