# Build Stage
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production Stage
FROM nginx:alpine

# Set default API URL if not provided
ENV API_URL=http://ugenix-api:8080

COPY --from=build /app/dist /usr/share/nginx/html

# Use a template for Nginx config to allow environment variable injection
COPY nginx.conf /etc/nginx/templates/default.conf.template

EXPOSE 80

# Nginx Alpine image automatically runs envsubst on files in /etc/nginx/templates/*.template 
# and outputs them to /etc/nginx/conf.d/ before starting nginx
CMD ["nginx", "-g", "daemon off;"]
