var myapp = angular.module('myapp', []);
myapp.controller('maincontroller', function ($scope, $http) {
    $http.get('/Home/GetMaiCategory/').then(function (response) {
        $scope.mainCategory = response.data;
    })
})
myapp.controller('childcontroller', function ($scope, $http) {
    $scope.getCategory = function (id) {
        $http.get('/Home/GetCategory/' + id).then(function (response) {
            $scope.cities = response.data;
        })
    }

})
myapp.controller('childcontroller', function ($scope, $http) {
    $scope.getCategory = function (id) {
        $http.get('/Home/GetCategory/' + id).then(function (response) {
            $scope.cities = response.data;
        })
    }

})
myapp.controller('childcontroller', function ($scope, $http) {
    $scope.getCategory = function (id) {
        $http.get('/Home/GetCategory/' + id).then(function (response) {
            $scope.cities = response.data;
        })
    }

})