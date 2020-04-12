FROM openjdk:8-jdk

# Just matched `app/build.gradle`
ENV ANDROID_COMPILE_SDK "29"
# Just matched `app/build.gradle`
ENV ANDROID_BUILD_TOOLS "29.0.3"
# Version from https://developer.android.com/studio/releases/sdk-tools
ENV ANDROID_SDK_TOOLS "6200805"

ENV ANDROID_HOME /android-sdk-linux
ENV PATH="${PATH}:/android-sdk-linux/platform-tools/"

# install OS packages
RUN apt-get --quiet update --yes
RUN apt-get --quiet install --yes wget tar unzip lib32stdc++6 lib32z1 build-essential ruby ruby-dev
# We use this for xxd hex->binary
RUN apt-get --quiet install --yes vim-common
# install Android SDK
RUN wget --quiet --output-document=android-sdk.zip https://dl.google.com/android/repository/commandlinetools-linux-${ANDROID_SDK_TOOLS}_latest.zip
RUN unzip -d android-sdk-linux android-sdk.zip
## Must pass updated arguments to SDK manager after breaking change. Following SO answer https://stackoverflow.com/a/60454207
RUN echo y | android-sdk-linux/tools/bin/sdkmanager --sdk_root=${ANDROID_HOME} "platform-tools" "platforms;android-${ANDROID_COMPILE_SDK}" >/dev/null
RUN echo y | android-sdk-linux/tools/bin/sdkmanager --sdk_root=${ANDROID_HOME} "platform-tools" >/dev/null
RUN echo y | android-sdk-linux/tools/bin/sdkmanager --sdk_root=${ANDROID_HOME} "build-tools;${ANDROID_BUILD_TOOLS}" >/dev/null
RUN yes | android-sdk-linux/tools/bin/sdkmanager --sdk_root=${ANDROID_HOME} --licenses
# install Fastlane
COPY Gemfile.lock .
COPY Gemfile .
RUN gem install bundle
RUN bundle install
