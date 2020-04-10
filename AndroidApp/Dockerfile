FROM menny/android:1.13.6 

# install OS packages
RUN apt-get --quiet update --yes
RUN apt-get --quiet install --yes ruby ruby-dev
# We use this for xxd hex->binary
RUN apt-get --quiet install --yes vim-common
# To make sure the local.properties file exists. See https://gitlab.com/edgsolutions-engineering/noon-online/-/issues/34#note_315811746
RUN touch local.properties
# install Fastlane
COPY Gemfile.lock .
COPY Gemfile .
RUN gem install bundle
RUN bundle install
