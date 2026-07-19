# heroku container:login

# push 時に error from registry: unsupported というエラー
# Docker Desktop の "Use containerd for pulling and storing images" をオフに
task :push do
  cp "server/Dockerfile", "Dockerfile"
  # sh "env BUILDX_NO_DEFAULT_ATTESTATIONS=1 heroku container:push web -a mentsuke"
  sh "\
    docker buildx build \
    --platform linux/amd64 \
    --provenance=false \
    -t registry.heroku.com/mentsuke/web \
    . \
  "
  sh "docker push registry.heroku.com/mentsuke/web"
end

task :release do
  cp "server/Dockerfile", "Dockerfile"
  sh "heroku container:release web -a mentsuke"
end
