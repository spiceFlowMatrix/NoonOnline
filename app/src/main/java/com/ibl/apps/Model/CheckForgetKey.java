package com.ibl.apps.Model;

public class CheckForgetKey {

    private String response_code;

    private Data data;

    private String message;

    private String status;

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    @Override
    public String toString() {
        return "ClassPojo [response_code = " + response_code + ", data = " + data + ", message = " + message + ", status = " + status + "]";
    }

    public class Data {

        private String password;

        private String confirmPassword;

        private String isforgot;

        private String email;

        private String authId;

        private String forgotkey;

        public String getPassword() {
            return password;
        }

        public void setPassword(String password) {
            this.password = password;
        }

        public String getConfirmPassword() {
            return confirmPassword;
        }

        public void setConfirmPassword(String confirmPassword) {
            this.confirmPassword = confirmPassword;
        }

        public String getIsforgot() {
            return isforgot;
        }

        public void setIsforgot(String isforgot) {
            this.isforgot = isforgot;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public String getAuthId() {
            return authId;
        }

        public void setAuthId(String authId) {
            this.authId = authId;
        }

        public String getForgotkey() {
            return forgotkey;
        }

        public void setForgotkey(String forgotkey) {
            this.forgotkey = forgotkey;
        }

        @Override
        public String toString() {
            return "ClassPojo [password = " + password + ", confirmPassword = " + confirmPassword + ", isforgot = " + isforgot + ", email = " + email + ", authId = " + authId + ", forgotkey = " + forgotkey + "]";
        }
    }
}

			