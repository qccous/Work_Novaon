package com.onfluencerusertracking;

import android.content.Intent;
import android.os.Bundle;
import com.facebook.react.HeadlessJsTaskService;
import com.facebook.react.bridge.Arguments;
import com.facebook.react.jstasks.HeadlessJsTaskConfig;
import javax.annotation.Nullable;

public class TrackingService extends HeadlessJsTaskService {
  @Override
  protected @Nullable HeadlessJsTaskConfig getTaskConfig(Intent intent) {
    Bundle extras = intent.getExtras();
    return new HeadlessJsTaskConfig(
      "Tracking", //JS function to call
      extras != null ? Arguments.fromBundle(extras) : null,
      5000,
      true);
  }
}