angular.module('workfront-addin')
.controller('LoginController', ['$scope', function($scope) {
    $scope.username = '';
    $scope.password = '';
    $scope.url = '';

    $('#myModal').modal({ backdrop: 'static' });
}])