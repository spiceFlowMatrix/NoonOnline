#!/bin/bash

# Exit on any error
set -e

/opt/google-cloud-sdk/bin/gcloud docker -- push eu.gcr.io/${GOOGLE_PROJECT_ID}/core-api-test:${CIRCLE_SHA1}
chown -R ubuntu:ubuntu /home/ubuntu/.kube