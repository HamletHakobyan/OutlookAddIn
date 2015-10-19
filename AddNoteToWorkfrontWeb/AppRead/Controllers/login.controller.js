(function(angular) {
    function loginController($scope, $modalInstance, $location, loginService) {
        $scope.username = '';
        $scope.password = '';
        $scope.url = '';
        $scope.isError = 'false';
        $scope.error = '';

        $scope.signIn = function() {
            var cnnInfo = {
                Username: $scope.username,
                Password: $scope.password,
                Host: $scope.url
            };

            $scope.isError = false;
            $scope.error = '';

            loginService.doLogin(cnnInfo).then(function(response) {
                    $modalInstance.close();
                    //$location.path('/update');
                },
                function(response) {
                    $scope.isError = true;
                    if (response.data && response.data.ExceptionMessage) {
                        $scope.error = response.data.ExceptionMessage;
                    } else if (response.data
                        && response.data.InnerException
                        && response.data.InnerException.ExceptionMessage) {
                        $scope.error = response.data.InnerException.ExceptionMessage;
                    } else {
                        $scope.error = response.statusText;
                    }
                });
        }
    }

    function modalController($scope, $uibModal) {
        $uibModal.open({
            templateUrl: 'templates/login/login.view.html',
            controller: 'LoginController',
            backdrop: 'static'
        });
    }

    angular.module('workfront-addin')
        .controller('LoginController', loginController)
        .controller('ModalController', modalController);


    loginController.$inject = ['$scope', '$modalInstance', '$location', 'loginService'];
    modalController.$inject = ['$scope', '$uibModal'];
})(window.angular);