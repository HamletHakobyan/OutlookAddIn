angular.module('workfront-addin')
    .controller('LoginController', [
        '$scope', '$modalInstance', '$location', 'loginService', function($scope, $modalInstance, $location, loginService) {
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

                loginService.doLogin(cnnInfo).then(function (response) {
                        $modalInstance.close();
                        $location.path('/update');
                    },
                    function(response) {
                        $scope.isError = true;
                        if (response.data && response.data.ExceptionMessage) {
                            $scope.error = response.data.ExceptionMessage;
                        } else {
                            $scope.error = response.statusText;
                        }
                    });
            }
        }
    ])
    .controller('ModalController', [
        '$scope', '$uibModal', function($scope, $uibModal) {
            $uibModal.open({
                templateUrl: 'templates/login/login.view.html',
                controller: 'LoginController',
                backdrop: 'static'
            });
        }
    ]);