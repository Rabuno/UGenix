# Build Stage
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production Stage
FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html

# Copy nginx config directly to a temporary location
COPY nginx.conf /etc/nginx/nginx.conf.template

EXPOSE 80

# Use a shell script to replace placeholder and start nginx
CMD ["/bin/sh", "-c", "sed -i \"s|__API_URL__|${API_URL}|g\" /etc/nginx/nginx.conf.template && cp /etc/nginx/nginx.conf.template /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
