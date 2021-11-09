# structured-logging-demo
An app that uses structured logging with dockerized Elastic Logstash stack

## Elastic stack setup for demo purposes
App sends logs to logstash, which adds them to `demo_app` index. If it isn't created, it will be created automatically. However, if you want to search logs using Discover in Kibana, you need to:

0. Start Elastic stack by running 
   ```
   docker compose up
   ```
1. After Kibana logs that it's ready, go to http://localhost:5601 (if it's not opening, it may be that Kibana hasn't started completely)
2. Start the app to write some logs to elastic
3. Skip (or go through) Kibana tutorial and go to Discover in side panel
4. Kibana will redirect to configuration page, asking to create an Index pattern, click create index
5. During configuration set the pattern to `demo_app*` and the timestamp field to `Timestamp` (without `@` - that's a timestamp when log was written by logstash)
6. Finish the configuration, go to Discover and test it