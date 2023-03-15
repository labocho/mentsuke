task :push do
  cp "server/Dockerfile", "Dockerfile"
  sh "heroku container:push web -a mentsuke"
end

task :release do
  cp "server/Dockerfile", "Dockerfile"
  sh "heroku container:release web -a mentsuke"
end
