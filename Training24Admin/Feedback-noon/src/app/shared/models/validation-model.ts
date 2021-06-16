export let EmailPattern = "^[a-zA-z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
export let UernamePattern = "^[a-zA-Z0-9.\-_$@*!]{3,30}$";
export let PasswordPattern = "^(?=.*\d)(?=.*[a-z])(?=.*[$@$!%*#?&])(?=.*[A-Z])[0-9a-zA-Z$@$!%*#?&]{8,}$";
export let UrlPattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
