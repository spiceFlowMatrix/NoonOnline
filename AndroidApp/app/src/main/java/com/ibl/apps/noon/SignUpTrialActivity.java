package com.ibl.apps.noon;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.annotation.NonNull;

import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseUser;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.noon.databinding.ActivitySignUpTrialBinding;
import com.ibl.apps.util.Validator;

public class SignUpTrialActivity extends BaseActivity implements View.OnClickListener {
    ActivitySignUpTrialBinding binding;
    private FirebaseAuth firebaseAuth;
    FirebaseAuth.AuthStateListener authStateListener;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_sign_up_trial;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivitySignUpTrialBinding) getBindObj();
        firebaseAuth = FirebaseAuth.getInstance();

        setOnClickListener();
    }

    private void setOnClickListener() {
        binding.txtAlreadyAccount.setOnClickListener(this);
        binding.cardviewsignupTrial.setOnClickListener(this);
    }

    @Override
    public void onStart() {
        super.onStart();
        // Check if user is signed in (non-null) and update UI accordingly.
        FirebaseUser currentUser = firebaseAuth.getCurrentUser();
//        updateUI(currentUser);
    }



    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.txtAlreadyAccount:
                openActivity(LoginActivity.class);
                break;

            case R.id.cardviewsignupTrial:
                if (validateFields()) {
                    String email = binding.signupEmail.getText().toString().trim();
                    String password = binding.signupPassword.getText().toString().trim();
                    String firstName = binding.signupFirstName.getText().toString().trim();
                    String lastName = binding.txtSignupLastName.getText().toString().trim();

                    showDialog(getString(R.string.loading));
                    firebaseAuth.createUserWithEmailAndPassword(email, password)
                            .addOnCompleteListener(new OnCompleteListener<AuthResult>() {
                                @Override
                                public void onComplete(@NonNull Task<AuthResult> task) {
                                    /*if (task.isSuccessful()) {
                                        Toast.makeText(getApplicationContext(), "Registration successful!", Toast.LENGTH_LONG).show();
                                        // progressBar.setVisibility(View.GONE);
                                        hideDialog();

                                        Intent intent = new Intent(SignUpTrialActivity.this, MainDashBoardTrialActivity.class);
                                        startActivity(intent);
                                    } else {
                                        Log.e("TAG", "onComplete: " + task.getException().getMessage());
                                        Toast.makeText(getApplicationContext(), "Registration failed! Please try again later", Toast.LENGTH_LONG).show();
                                        hideDialog();
                                    }*/

                                    if (task.isSuccessful()) {
                                        // Sign in success, update UI with the signed-in user's information
                                        Log.d("TAG", "createUserWithEmail:success");
                                        FirebaseUser user = FirebaseAuth.getInstance().getCurrentUser();
                                        if (user != null) {
                                            // Name, email address, and profile photo Url
                                            String name = user.getDisplayName();
                                            String email = user.getEmail();
                                            Uri photoUrl = user.getPhotoUrl();

                                            // Check if user's email is verified
                                            boolean emailVerified = user.isEmailVerified();

                                            // The user's ID, unique to the Firebase project. Do NOT use this value to
                                            // authenticate with your backend server, if you have one. Use
                                            // FirebaseUser.getIdToken() instead.
                                            String uid = user.getUid();
                                        }
                                        Intent intent = new Intent(SignUpTrialActivity.this, MainDashBoardTrialActivity.class);
                                        startActivity(intent);
                                    } else {
                                        // If sign in fails, display a message to the user.
                                        Log.w("TAG", "createUserWithEmail:failure", task.getException());
                                        Toast.makeText(SignUpTrialActivity.this, "Authentication failed.",
                                                Toast.LENGTH_SHORT).show();
//                                        updateUI(null);
                                    }
                                }
                            });


                }
                break;
        }
    }

    private boolean validateFields() {

        //For Email Validation
        if (!Validator.checkEmpty(binding.signupEmail)) {
            hideKeyBoard(binding.signupEmail);
            binding.signupEmailWrapper.setError(getString(R.string.validation_enterEmail));
            return false;
        } else {
            hideKeyBoard(binding.signupEmail);
            binding.signupEmailWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmail(binding.signupEmail)) {
            hideKeyBoard(binding.signupEmail);
            binding.signupEmailWrapper.setError(getString(R.string.validation_validEmail));
            return false;
        } else {
            hideKeyBoard(binding.signupEmail);
            binding.signupEmailWrapper.setErrorEnabled(false);
        }

        //For Password Validation
        if (!Validator.checkEmpty(binding.signupPassword)) {
            hideKeyBoard(binding.signupPassword);
            binding.signupPasswordWrapper.setError(getString(R.string.validation_enterPassword));
            return false;
        } else {
            hideKeyBoard(binding.signupPassword);
            binding.signupPasswordWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkLength(binding.signupPassword)) {
            hideKeyBoard(binding.signupPassword);
            binding.signupPasswordWrapper.setError(getString(R.string.validation_passwordLenght));
            return false;
        } else {
            hideKeyBoard(binding.signupPassword);
            binding.signupPasswordWrapper.setErrorEnabled(false);
        }

        //For Name Validation
        if (!Validator.checkEmpty(binding.signupFirstName)) {
            hideKeyBoard(binding.signupFirstName);
            binding.signupNameWrapper.setError(getString(R.string.validation_enterName));
            return false;
        } else {
            hideKeyBoard(binding.signupFirstName);
            binding.signupNameWrapper.setErrorEnabled(false);
        }

        if (!Validator.checkEmpty(binding.txtSignupLastName)) {
            hideKeyBoard(binding.txtSignupLastName);
            binding.signupLastWrapper.setError(getString(R.string.validation_enterFatherNameSurname));
            return false;
        } else {
            hideKeyBoard(binding.txtSignupLastName);
            binding.signupLastWrapper.setErrorEnabled(false);
        }
        return true;
    }

}
