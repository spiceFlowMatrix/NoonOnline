package com.ibl.apps.Model;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.List;

/**
 * Created by iblinfotech on 05/12/18.
 */
@Entity
public class AuthTokenObject {

    @PrimaryKey(autoGenerate = true)
    private int AuthTokenAutoId;

    private String accessToken;

    private String type;

    private String idToken;

    private String refreshToken;

    private Long expiresIn;

    private String scope;

    private String expiresAt;

    private String exp;

    private String sub;

    private String aud;

    private String iss;

    private String iat;

    private String username;

    @ColumnInfo(name = "roles")
    @TypeConverters(DataTypeConverter.class)
    private List<String> roles;

    /*@Nullable
    public int mypubYear;

    @Nullable
    public int getMypubYear() {
        return mypubYear;
    }

    public void setMypubYear(@Nullable int mypubYear) {
        this.mypubYear = mypubYear;
    }
*/
    public int getAuthTokenAutoId() {
        return AuthTokenAutoId;
    }

    public void setAuthTokenAutoId(int authTokenAutoId) {
        AuthTokenAutoId = authTokenAutoId;
    }

    public String getAccessToken() {
        return accessToken;
    }

    public void setAccessToken(String accessToken) {
        this.accessToken = accessToken;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getIdToken() {
        return idToken;
    }

    public void setIdToken(String idToken) {
        this.idToken = idToken;
    }

    public String getRefreshToken() {
        return refreshToken;
    }

    public void setRefreshToken(String refreshToken) {
        this.refreshToken = refreshToken;
    }

    public Long getExpiresIn() {
        return expiresIn;
    }

    public void setExpiresIn(Long expiresIn) {
        this.expiresIn = expiresIn;
    }

    public String getScope() {
        return scope;
    }

    public void setScope(String scope) {
        this.scope = scope;
    }

    public String getExpiresAt() {
        return expiresAt;
    }

    public void setExpiresAt(String expiresAt) {
        this.expiresAt = expiresAt;
    }

    public String getExp() {
        return exp;
    }

    public void setExp(String exp) {
        this.exp = exp;
    }

    public String getSub() {
        return sub;
    }

    public void setSub(String sub) {
        this.sub = sub;
    }

    public String getAud() {
        return aud;
    }

    public void setAud(String aud) {
        this.aud = aud;
    }

    public String getIss() {
        return iss;
    }

    public void setIss(String iss) {
        this.iss = iss;
    }

    public String getIat() {
        return iat;
    }

    public void setIat(String iat) {
        this.iat = iat;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public List<String> getRoles() {
        return roles;
    }

    public void setRoles(List<String> roles) {
        this.roles = roles;
    }


    @Override
    public String toString() {
        return "AuthTokenObject{" +
                "AuthTokenAutoId=" + AuthTokenAutoId +
                ", accessToken='" + accessToken + '\'' +
                ", type='" + type + '\'' +
                ", idToken='" + idToken + '\'' +
                ", refreshToken='" + refreshToken + '\'' +
                ", expiresIn=" + expiresIn +
                ", scope='" + scope + '\'' +
                ", expiresAt='" + expiresAt + '\'' +
                ", exp='" + exp + '\'' +
                ", sub='" + sub + '\'' +
                ", aud='" + aud + '\'' +
                ", iss='" + iss + '\'' +
                ", iat='" + iat + '\'' +
                ", username='" + username + '\'' +
                ", roles=" + roles +
                '}';
    }
}
