### STAGE 1: Build ###

FROM node:latest as build-env

COPY package.json package-lock.json ./

## Storing node modules on a separate layer will prevent unnecessary npm installs at each build

RUN npm ci && mkdir /ng-app && mv ./node_modules ./ng-app

WORKDIR /ng-app

COPY . .

## Build the angular app
RUN npm run ng build -- --output-path=dist


### STAGE 2: Setup ###

FROM nginx:latest

## Copy our default nginx config
COPY .nginx/default.conf /etc/nginx/conf.d/

## Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

## Copy over the artifacts
COPY --from=build-env /ng-app/dist /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
