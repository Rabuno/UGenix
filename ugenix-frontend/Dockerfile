# Build Stage
FROM node:20-alpine AS build
ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=$VITE_API_BASE_URL
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

# Use a shell script to replace placeholders (API URL and DNS Resolver) and start nginx
CMD ["/bin/sh", "-c", "export NAMESERVER=$(grep nameserver /etc/resolv.conf | awk '{print $2}' | head -n 1) && sed -i \"s|__API_URL__|${API_URL}|g\" /etc/nginx/nginx.conf.template && sed -i \"s|__NAMESERVER__|${NAMESERVER}|g\" /etc/nginx/nginx.conf.template && cp /etc/nginx/nginx.conf.template /etc/nginx/conf.d/default.conf && nginx -g 'daemon off;'"]
