angular.module('workfront-addin')
.controller('LoginController', ['$scope', '$modalInstance', function ($scope, $modalInstance) {
    $scope.username = '';
    $scope.password = '';
    $scope.url = '';

    $scope.signIn = function () {
        var cnnInfo = {
            Username: $scope.username,
            Password: $scope.password,
            Host: $scope.url
        };

        doLogin(cnnInfo);

    }

    var doLogin = function (connectionInfo) {
        $(".loading").show(1000);

        $.post("../../api/authentication/login", connectionInfo)
            .done(function (data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .fail(function (data, status) {
                $("#jsonResponse").text("Data: " + JSON.stringify(data) + "\nStatus: " + status);
            })
            .always(function () {
                $(".loading").hide(1000);
            });
    }
}])
.controller('ModalController', ['$scope', '$uibModal', function ($scope, $uibModal) {
        $uibModal.open({
            templateUrl: 'templates/login/login.view.html',
            controller: 'LoginController',
            backdrop: 'static'
        });
    }]);