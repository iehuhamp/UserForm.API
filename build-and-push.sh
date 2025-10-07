#!/bin/bash

# Configuration
DOCKER_USERNAME="adsevertest" 
IMAGE_NAME="userform-api"
VERSION="latest"

# Full image tag
FULL_IMAGE_TAG="$DOCKER_USERNAME/$IMAGE_NAME:$VERSION"

echo "Building Docker image: $FULL_IMAGE_TAG"
docker build -t $FULL_IMAGE_TAG .

if [ $? -ne 0 ]; then
    echo "Docker build failed!"
    exit 1
fi

echo "Build successful!"

# Login to Docker Hub
echo "Logging in to Docker Hub..."
docker login

if [ $? -ne 0 ]; then
    echo "Docker login failed!"
    exit 1
fi

# Push image to Docker Hub
echo "Pushing image to Docker Hub: $FULL_IMAGE_TAG"
docker push $FULL_IMAGE_TAG

if [ $? -ne 0 ]; then
    echo "Docker push failed!"
    exit 1
fi

echo "Successfully pushed $FULL_IMAGE_TAG to Docker Hub!"
